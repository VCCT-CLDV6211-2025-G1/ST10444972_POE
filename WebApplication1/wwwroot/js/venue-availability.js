function formatDateTime(date) {
    return new Date(date).toLocaleString(undefined, {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}

function showError(message, containerId = 'venueConflicts') {
    const container = document.getElementById(containerId);
    container.innerHTML = `<div class="alert alert-danger mt-3">${message}</div>`;
    container.style.display = 'block';
}

function showSuccess(containerId = 'venueConflicts') {
    const container = document.getElementById(containerId);
    container.innerHTML = '<div class="alert alert-success mt-3">Venue is available for the selected dates!</div>';
    container.style.display = 'block';
}

function showConflicts(conflicts, containerId = 'venueConflicts') {
    const container = document.getElementById(containerId);
    let conflictHtml = '<div class="alert alert-warning mt-3"><h5>Venue Already Booked:</h5><ul>';
    
    conflicts.forEach(conflict => {
        const startDate = new Date(conflict.startDate).toLocaleDateString();
        const endDate = new Date(conflict.endDate).toLocaleDateString();
        const timeRange = `${new Date(conflict.startDate).toLocaleTimeString([], {hour: '2-digit', minute:'2-digit'})} - ${new Date(conflict.endDate).toLocaleTimeString([], {hour: '2-digit', minute:'2-digit'})}`;
        
        conflictHtml += `
            <li>
                <strong>${conflict.eventName}</strong>
                <br/>
                <small class="text-muted">Date: ${startDate === endDate ? startDate : `${startDate} - ${endDate}`}</small>
                <br/>
                <small class="text-muted">Time: ${timeRange}</small>
            </li>`;
    });
    
    conflictHtml += '</ul><p><small>Only one event per day is allowed per venue.</small></div>';
    container.innerHTML = conflictHtml;
    container.style.display = 'block';
}

function validateDates(startDate, endDate) {
    const start = new Date(startDate);
    const end = new Date(endDate);
    const now = new Date();

    // Remove time part for date comparison
    now.setHours(0, 0, 0, 0);
    start.setHours(0, 0, 0, 0);

    if (start < now) {
        return "Start date cannot be in the past";
    }

    if (end <= start) {
        return "End date must be after start date";
    }

    return null;
}

async function checkVenueAvailability(venueId, startDate, endDate, eventId = null) {
    if (!venueId || !startDate || !endDate) {
        return;
    }

    const validationError = validateDates(startDate, endDate);
    if (validationError) {
        showError(validationError);
        return;
    }

    const params = new URLSearchParams({
        venueId: venueId,
        startDate: startDate,
        endDate: endDate
    });

    if (eventId) {
        params.append('excludeEventId', eventId);
    }

    try {
        const response = await fetch(`/api/VenueAvailability/check?${params}`);
        
        if (!response.ok) {
            if (response.status === 400) {
                const errorText = await response.text();
                showError(errorText);
            } else {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return;
        }

        const data = await response.json();
        
        if (!data.isAvailable) {
            showConflicts(data.conflicts);
        } else {
            showSuccess();
        }
    } catch (error) {
        console.error('Error checking venue availability:', error);
        showError('Error checking venue availability. Please try again.');
    }
}

async function showBookedDates(venueId, eventId = null) {
    if (!venueId) return;
    
    try {
        const params = new URLSearchParams({ excludeEventId: eventId || '' });
        const response = await fetch(`/api/VenueAvailability/booked-dates/${venueId}?${params}`);
        const bookedDates = await response.json();
        
        let bookedDatesDiv = document.getElementById('bookedDates');
        if (!bookedDatesDiv) {
            bookedDatesDiv = document.createElement('div');
            bookedDatesDiv.id = 'bookedDates';
            document.getElementById('VenueId').parentNode.appendChild(bookedDatesDiv);
        }
        
        if (bookedDates.length > 0) {
            bookedDatesDiv.innerHTML = `
                <div class="alert alert-info mt-2">
                    <small><strong>Booked Dates:</strong> ${bookedDates.join(', ')}</small>
                </div>`;
        } else {
            bookedDatesDiv.innerHTML = '';
        }
    } catch (error) {
        console.error('Error fetching booked dates:', error);
    }
}

function setupAvailabilityCheck(eventId = null) {
    const venueSelect = document.getElementById('VenueId');
    const startDateInput = document.getElementById('StartDate');
    const endDateInput = document.getElementById('EndDate');
    
    // Create conflicts container if it doesn't exist
    let conflictsDiv = document.getElementById('venueConflicts');
    if (!conflictsDiv) {
        conflictsDiv = document.createElement('div');
        conflictsDiv.id = 'venueConflicts';
        conflictsDiv.style.display = 'none';
        venueSelect.parentNode.appendChild(conflictsDiv);
    }

    function checkAvailability() {
        const venueId = venueSelect.value;
        const startDate = startDateInput.value;
        const endDate = endDateInput.value;
        
        if (venueId && startDate && endDate) {
            checkVenueAvailability(venueId, startDate, endDate, eventId);
        } else {
            conflictsDiv.style.display = 'none';
        }
    }

    // Add debounce to prevent too many API calls
    let timeoutId;
    function debouncedCheck() {
        clearTimeout(timeoutId);
        timeoutId = setTimeout(checkAvailability, 500);
    }

    venueSelect.addEventListener('change', function() {
        showBookedDates(this.value, eventId);
        debouncedCheck();
    });
    startDateInput.addEventListener('change', debouncedCheck);
    endDateInput.addEventListener('change', debouncedCheck);

    // Show booked dates and check availability on page load
    if (venueSelect.value) {
        showBookedDates(venueSelect.value, eventId);
        if (startDateInput.value && endDateInput.value) {
            checkAvailability();
        }
    }
}

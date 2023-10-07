/* Script taken from https://github.com/niinpatel/calendarHTML-Javascript/blob/master/scripts.js
 * By Nitin Patel, licenced under MIT
 * Modifications (and cleanup) made for ASP.NET by Jai Mu for HIT339.
 */

let currentMonth;
let currentYear;
let selectYear;
let selectMonth;

function initCalendar() {
    let today = new Date();
    currentMonth = today.getMonth();
    currentYear = today.getFullYear();
    selectYear = document.getElementById("year");
    selectMonth = document.getElementById("month");

    let months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    let monthAndYear = document.getElementById("monthAndYear");

    // Errors with an "Uncaught TypeError: $(...).tooltip is not a function" if you don't wait for the page to load. 
    document.addEventListener("DOMContentLoaded", function () {
        showCalendar(currentMonth, currentYear);
    });
}

function next() {
    currentYear = (currentMonth === 11) ? currentYear + 1 : currentYear;
    currentMonth = (currentMonth + 1) % 12;
    showCalendar(currentMonth, currentYear);
}

function previous() {
    currentYear = (currentMonth === 0) ? currentYear - 1 : currentYear;
    currentMonth = (currentMonth === 0) ? 11 : currentMonth - 1;
    showCalendar(currentMonth, currentYear);
}

function jump() {
    currentYear = parseInt(selectYear.value);
    currentMonth = parseInt(selectMonth.value);
    showCalendar(currentMonth, currentYear);
}

function showCalendar(month, year) {
    let firstDay = (new Date(year, month)).getDay();
    let daysInMonth = 32 - new Date(year, month, 32).getDate();

    let tbl = document.getElementById("calendar-body"); // body of the calendar

    // clearing all previous cells
    tbl.innerHTML = "";

    // filing data about month and in the page via DOM.
    monthAndYear.innerHTML = months[month] + " " + year;
    selectYear.value = year;
    selectMonth.value = month;

    // creating all cells
    let date = 1;
    for (let i = 0; i < 6; i++) {
        // creates a table row
        let row = document.createElement("tr");

        //creating individual cells, filing them up with data.
        for (let j = 0; j < 7; j++) {
            let cell = document.createElement("td");
            if (i === 0 && j < firstDay) {
                let cellText = document.createTextNode("");
                cell.appendChild(cellText);
            }
            else if (date > daysInMonth) {
                break;
            }
            else {
                let cellText = document.createTextNode(date);
                if (date === today.getDate() && year === today.getFullYear() && month === today.getMonth()) {
                    cell.classList.add("bg-info"); // color today's date
                }
                cell.appendChild(cellText);
                cell.id = date; // Add an id to the cell
                date++;
            }
            row.appendChild(cell);
        }

        tbl.appendChild(row); // appending each row into calendar body.
    }

    // Clear the event list
    let eventList = document.getElementById("event-list");
    eventList.innerHTML = "";

    // Fetch schedules from ASP.NET server
    fetch('/Schedules/GetSchedules')
        .then(response => response.json())
        .then(schedules => {
            console.log('Received schedules:', schedules);
            // Loop and add them to the calendar
            for (let schedule of schedules) {
                // Convert the schedule date from JSON to JavaScript Date object
                let scheduleDate = new Date(schedule.date);

                console.log('Processing schedule with date:', scheduleDate);

                // Check if the schedule is in the current month
                if (scheduleDate.getMonth() === month && scheduleDate.getFullYear() === year) {
                    // Add the schedule to the calendar
                    let cell = document.getElementById(scheduleDate.getDate());

                    // Highlight the cell with events
                    cell.classList.add("bg-warning");

                    // Create a list item for the event details
                    let listItem = document.createElement("li");
                    listItem.classList.add("list-group-item");

                    // Create a link to the event details page
                    let eventLink = document.createElement("a");
                    eventLink.href = "/Schedules/Details/" + schedule.scheduleId;
                    eventLink.innerText = schedule.eventName + " (" + schedule.date + ")";
                    listItem.appendChild(eventLink);

                    // Add the list item to the event list
                    eventList.appendChild(listItem);
                }
            }
        }
        );
}

document.addEventListener("DOMContentLoaded", function ()
{
    initCalendar();
});
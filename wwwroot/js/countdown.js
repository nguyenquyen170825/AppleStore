// Flash Sale Countdown Timer
let endtime = new Date().getTime() + 10 * 60 * 1000;

function countdown() {
    let nowtime = new Date().getTime();
    let distance = endtime - nowtime;

    if (distance <= 0) {
        document.getElementById("h").textContent = "00";
        document.getElementById("m").textContent = "00";
        document.getElementById("s").textContent = "00";
        return;
    }

    let hours = Math.floor(distance / (1000 * 60 * 60));
    let minutes = Math.floor((distance % (60 * 60 * 1000)) / (60 * 1000));
    let seconds = Math.floor((distance % (60 * 1000)) / 1000);

    // Pad with leading zeros for clean display
    document.getElementById("h").textContent = String(hours).padStart(2, '0');
    document.getElementById("m").textContent = String(minutes).padStart(2, '0');
    document.getElementById("s").textContent = String(seconds).padStart(2, '0');

    setTimeout(countdown, 1000);
}

countdown();
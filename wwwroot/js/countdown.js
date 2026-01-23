let endtime=new Date().getTime() + 10*60*1000;
function countdown(){
    let nowtime=new Date().getTime();
    let distance=endtime-nowtime;
    if(distance<=0){
        document.querySelector('.countdown').innerHTML="HẾT GIỜ";
        return;
    }
    let hours=Math.floor(distance/(1000*60*60));
    let minutes=Math.floor((distance%(60*60*1000))/(60*1000));
    let seconds=Math.floor((distance%(60*1000))/1000);
    // document.querySelector('.countdown').innerHTML=`${hours}h ${minutes}m ${seconds}s`;
    document.getElementById("h").textContent = hours;
    document.getElementById("m").textContent = minutes;
    document.getElementById("s").textContent = seconds;
    setTimeout(countdown,1000);
}
countdown();
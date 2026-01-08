let endtime=new Date().getTime();
console.log(endtime);
function countdown(){
    let nowtime=new Date().getTime();
    let distance=endtime-nowtime;
    if(distance<=0){
        document.querySelector('.countdown').innerHTML="HẾT GIỜ";
        return;
    }
    
}
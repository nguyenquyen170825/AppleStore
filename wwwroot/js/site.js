// const btnDanhMuc = document.querySelector('.btnCategory'); // Nút bấm của bạn
// const overlay = document.getElementById('menu-category');
// const menu = document.querySelector('.categoryDropdown');

// btnDanhMuc.addEventListener('click', function() {
//     overlay.style.display = 'block';
//     menu.style.display = 'block';
// });

// // Khi nhấn vào lớp mờ thì đóng menu
// overlay.addEventListener('click', function() {
//     overlay.style.display = 'none';
//     menu.style.display = 'none';
// });

document.getElementById("btnCategory").onclick = function () {

    let menu = document.getElementById("categoryDropdown");

    if(menu.style.display == "block"){
        menu.style.display = "none";
    }
    else{
        menu.style.display = "block";
    }

}
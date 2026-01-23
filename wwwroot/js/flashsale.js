fetch('/sanpham/GetFlashSale')
    .then(res=> res.json())
    .then(data =>{renderflashsale(data)});

function renderflashsale(list){
    const container= document.getElementById('flashSaleProducts'); // Thay đổi ID container
    container.innerHTML='';

    list.forEach(sp => {
        container.innerHTML += `
            <div class="product-card">
                <img src="${sp.hinhAnh}" alt="${sp.ten}">
                <h3>${sp.ten}</h3>
                <p>Giá: ${sp.gia.toLocaleString()} đ</p>
                <p>Giảm: ${sp.giamgia}%</p>
                <p>Còn lại: ${sp.soluong}</p>
            </div>
        `;
    });

    // Thêm chức năng cuộn sản phẩm khi nhấn mũi tên
    const scrollContainer = document.getElementById('flashSaleProducts');
    const leftArrow = document.querySelector('.left-arrow');
    const rightArrow = document.querySelector('.right-arrow');

    if (leftArrow && rightArrow && scrollContainer) {
        // Tính toán lượng cuộn bằng chiều rộng hiển thị của container để chuyển "trang" sản phẩm
        let scrollAmount = scrollContainer.clientWidth;

        // Thêm sự kiện click cho mũi tên trái
        leftArrow.addEventListener('click', () => {
            // Tính toán vị trí cuộn mục tiêu
            let targetScrollLeft = scrollContainer.scrollLeft - scrollAmount;
            // Đảm bảo không cuộn quá mức về phía đầu
            if (targetScrollLeft < 0) {
                targetScrollLeft = 0;
            }
            scrollContainer.scrollTo({ left: targetScrollLeft, behavior: 'smooth' });
        });

        // Thêm sự kiện click cho mũi tên phải
        rightArrow.addEventListener('click', () => {
            // Tính toán vị trí cuộn mục tiêu
            let targetScrollLeft = scrollContainer.scrollLeft + scrollAmount;
            // Đảm bảo không cuộn quá mức về phía cuối
            const maxScrollLeft = scrollContainer.scrollWidth - scrollContainer.clientWidth;
            if (targetScrollLeft > maxScrollLeft) {
                targetScrollLeft = maxScrollLeft;
            }
            scrollContainer.scrollTo({ left: targetScrollLeft, behavior: 'smooth' });
        });
    }
}
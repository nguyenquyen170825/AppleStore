fetch('/Product/GetFlashSale')
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

    
}
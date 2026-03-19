fetch('/Product/GetFlashSale')
    .then(res => res.json())
    .then(data => renderFlashSale(data));

function renderFlashSale(list) {

    const container = document.getElementById('flashSaleProducts');
    container.innerHTML = '';

    list.forEach(sp => {

        const gia = sp.gia ? sp.gia.toLocaleString() : '';
        const giamgia = sp.giamgia ?? 0;

        container.innerHTML += `
        <a href="/Product/Chitietsanpham/${sp.id}">
            <div class="product-card">

                <div class="product-image">
                    <img src="${sp.hinhAnh ?? '/images/no-image.png'}" alt="${sp.ten}">
                </div>

                <div class="product-info">

                    <h3>${sp.ten}</h3>

                    <div class="price">
                        ${gia} đ
                    </div>

                    ${giamgia > 0 ? `
                        <div class="discount-badge">
                            -${giamgia}%
                        </div>
                    ` : ''}

                    <div class="stock">
                        Còn lại: ${sp.soluong}
                    </div>

                </div>

            </div>
        </a>
        `;
    });

}
// ============================================
// SITE-WIDE SCRIPTS
// ============================================

// ---- Cart Badge: Fetch số lượng giỏ hàng ----
function updateCartBadge() {
    fetch('/Cart/GetCartCount')
        .then(res => res.json())
        .then(data => {
            const badge = document.getElementById('cartBadge');
            const cartBtn = document.getElementById('navCartBtn');

            if (!badge || !cartBtn) return;

            if (data.count > 0) {
                badge.textContent = data.count > 99 ? '99+' : data.count;
                badge.style.display = 'flex';
                cartBtn.classList.add('nav-cart-active');
            } else {
                badge.style.display = 'none';
                cartBtn.classList.remove('nav-cart-active');
            }
        })
        .catch(() => {
            // Silently fail - cart count is not critical
        });
}

// Gọi khi trang load
updateCartBadge();

// ---- Filter Dropdown Toggle (Hỗ trợ click/mobile) ----
document.addEventListener('click', function(e) {
    // Nếu click vào nút mở filter
    const filterBtn = e.target.closest('.filter-chip');
    if (filterBtn) {
        const parentFilter = filterBtn.closest('.filter');
        if (parentFilter) {
            // Đóng tất cả các filter khác đang mở
            document.querySelectorAll('.filter').forEach(f => {
                if (f !== parentFilter) f.classList.remove('filter-open');
            });
            // Toggle filter hiện tại
            parentFilter.classList.toggle('filter-open');
        }
        return;
    }

    // Nếu click ra ngoài vùng menu filter, đóng tất cả
    if (!e.target.closest('.filter-menu')) {
        document.querySelectorAll('.filter').forEach(f => {
            f.classList.remove('filter-open');
        });
    }
});

// ---- Toast Notification: Hiển thị thông báo premium ----
function showToast(message, isError = false) {
    const toast = document.getElementById('toastNotification');
    if (!toast) return;

    const span = toast.querySelector('span');
    const icon = toast.querySelector('i');
    
    if (span) span.textContent = message;
    if (icon) {
        icon.className = isError 
            ? 'fa-solid fa-circle-exclamation' 
            : 'fa-solid fa-circle-check';
    }
    
    toast.classList.toggle('toast-error', isError);
    toast.classList.add('show');
    
    // Tự động ẩn sau 3 giây
    setTimeout(() => {
        toast.classList.remove('show');
    }, 3000);
}
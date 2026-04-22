// ============================================
// HOMEPAGE INTERACTIVE SCRIPTS
// ============================================

// ---- Floating Particles ----
(function createParticles() {
    const container = document.getElementById('heroParticles');
    if (!container) return;

    const count = 25;
    for (let i = 0; i < count; i++) {
        const particle = document.createElement('div');
        particle.classList.add('particle');
        particle.style.left = Math.random() * 100 + '%';
        particle.style.top = Math.random() * 100 + '%';
        particle.style.width = (Math.random() * 4 + 2) + 'px';
        particle.style.height = particle.style.width;
        particle.style.animationDelay = (Math.random() * 6) + 's';
        particle.style.animationDuration = (Math.random() * 6 + 5) + 's';

        // Subtle pastel colors for light theme
        const colors = [
            'rgba(0, 113, 227, 0.15)',
            'rgba(110, 58, 255, 0.12)',
            'rgba(88, 86, 214, 0.14)',
            'rgba(0, 199, 190, 0.12)'
        ];
        particle.style.background = colors[Math.floor(Math.random() * colors.length)];

        container.appendChild(particle);
    }
})();

// ---- Scroll Reveal Animations ----
(function initScrollAnimations() {
    const sections = document.querySelectorAll(
        '.categories-section, .flashsale-section, .trending-section, .new-products-section, .promo-banner'
    );

    sections.forEach(section => {
        section.classList.add('fade-in-section');
    });

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
            }
        });
    }, {
        threshold: 0.12,
        rootMargin: '0px 0px -50px 0px'
    });

    sections.forEach(section => observer.observe(section));
})();

// ---- Flash Sale Navigation Arrows ----
(function initFlashSaleNav() {
    const leftBtn = document.getElementById('flashsaleNavLeft');
    const rightBtn = document.getElementById('flashsaleNavRight');
    const container = document.getElementById('flashSaleProducts');

    if (!leftBtn || !rightBtn || !container) return;

    const scrollAmount = 280;

    leftBtn.addEventListener('click', () => {
        container.scrollBy({ left: -scrollAmount, behavior: 'smooth' });
    });

    rightBtn.addEventListener('click', () => {
        container.scrollBy({ left: scrollAmount, behavior: 'smooth' });
    });
})();

// ---- Smooth scroll for scroll indicator ----
(function initSmoothScroll() {
    const scrollIndicator = document.querySelector('.hero-scroll-indicator');
    if (!scrollIndicator) return;

    scrollIndicator.addEventListener('click', () => {
        const target = document.getElementById('categories-section');
        if (target) {
            target.scrollIntoView({ behavior: 'smooth' });
        }
    });

    scrollIndicator.style.cursor = 'pointer';
})();

// ---- Parallax effect on hero ----
(function initHeroParallax() {
    const heroImage = document.querySelector('.hero-image');
    const heroGlow = document.querySelector('.hero-phone-glow');

    if (!heroImage) return;

    document.addEventListener('mousemove', (e) => {
        const x = (e.clientX / window.innerWidth - 0.5) * 20;
        const y = (e.clientY / window.innerHeight - 0.5) * 20;

        heroImage.style.transform = `translateY(${-9 + y * 0.3}px) translateX(${x * 0.3}px)`;

        if (heroGlow) {
            heroGlow.style.transform = `translate(${x * 0.5}px, ${y * 0.5}px)`;
        }
    });
})();

// ---- Category cards staggered animation ----
(function initCategoryAnimations() {
    const cards = document.querySelectorAll('.category-card');

    const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry, index) => {
            if (entry.isIntersecting) {
                setTimeout(() => {
                    entry.target.style.opacity = '1';
                    entry.target.style.transform = 'translateY(0)';
                }, index * 100);
            }
        });
    }, { threshold: 0.2 });

    cards.forEach(card => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(25px)';
        card.style.transition = 'all 0.6s cubic-bezier(0.25, 0.46, 0.45, 0.94)';
        observer.observe(card);
    });
})();

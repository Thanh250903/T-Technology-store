// Image preview script - included as a static JS file (no <script> tags)
document.addEventListener('DOMContentLoaded', function () {
    const input = document.getElementById('formFile');
    const thumb = document.getElementById('imgThumb');

    const overlay = document.getElementById('imageZoomOverlay');
    const zoomImg = document.getElementById('imageZoomImg');
    const closeBtn = document.getElementById('imageZoomClose');
    if (!input || !thumb) return;

    input.addEventListener('change', function () {
        const file = this.files && this.files[0];
        if (!file) {
            // user didn't pick anything; keep current thumb as-is
            return;
        }

        const url = URL.createObjectURL(file);
        thumb.classList.remove('d-none');
        thumb.src = url;
    });

    // Click thumb to zoom
    thumb.addEventListener('click', function () {
        if (!overlay || !zoomImg || !closeBtn) return;
        if (thumb.classList.contains('d-none')) return;
        if (!thumb.src) return;

        zoomImg.src = thumb.src;
        overlay.classList.add('is-open');
        overlay.setAttribute('aria-hidden', 'false');
    });

    // Close button
    if (closeBtn && overlay) {
        closeBtn.addEventListener('click', function () {
            overlay.classList.remove('is-open');
            overlay.setAttribute('aria-hidden', 'true');
        });

        // Click outside to close
        overlay.addEventListener('click', function (e) {
            if (e.target === overlay) {
                overlay.classList.remove('is-open');
                overlay.setAttribute('aria-hidden', 'true');
            }
        });

        // ESC to close
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape' && overlay.classList.contains('is-open')) {
                overlay.classList.remove('is-open');
                overlay.setAttribute('aria-hidden', 'true');
            }
        });
    }
});
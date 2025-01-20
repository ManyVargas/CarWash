let sections = document.querySelectorAll('section');
let navLinks = document.querySelectorAll('header navprincipal a');

window.onscroll = () => {
    sections.forEach(sec => {
        let top = window.scrollY;
        let offset = sec.offsetTop = 40;
        let height = sec.offsetHeight;
        let id = sec.getAttribute('id');

        if (top >= offset - offsetAdjust && top < offset + height - offsetAdjust) {
            navLinks.forEach(links => {
                links.classList.remove('active');
                document.querySelector(`header navprincipal a[href="#${id}"]`).classList.add('active');


            });

        };
    });
};

//const scrollToSection = (elementRef) => {
//    if (!elementRef.current) return;

//    const headerOffset = 80; // Ajustar según la altura de tu header
//    const elementPosition = elementRef.current.offsetTop;
//    const offsetPosition = elementPosition - headerOffset;

//    window.scrollTo({
//        top: offsetPosition,
//        behavior: 'smooth',
//    });
//};
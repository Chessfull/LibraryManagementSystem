![My Entities](https://github.com/user-attachments/assets/4360a4da-c407-4069-ab57-1c573e67f60c)

![MVC](https://github.com/user-attachments/assets/3701b368-e837-4b9b-9fd9-13650ee07da0)

<link rel="stylesheet" href="css/slider-css.css">
<script src="js/slider.js"></script>

<div class="slider" data-slider>
	<div class="slides">
		<div><img src="#"></div>
		<div><img src="#"></div>
		<div><video src="#"></div>
		<div>Text content could go here, too.</div>
	</div>
	<div data-slider-nav></div>
	<div data-slider-count></div>
</div>

<script>
	window.sliderInit = (function (window, document, undefined) {

		'use strict';

		// Feature Test
		if ( 'querySelector' in document && 'addEventListener' in window && Array.prototype.forEach ) {

			// SELECTORS
			var sliders = document.querySelectorAll('[data-slider]');
			var mySwipe = Array;


			// EVENTS, LISTENERS, AND INITS

			// Add class to HTML element to activate conditional CSS
			document.documentElement.className += ' js-slider';

			// Activate all sliders
			Array.prototype.forEach.call(sliders, function (slider, index) {

				// SELECTORS

				var slideCount = slider.querySelector('[data-slider-count]'); // Slider count wrapper
				var slideNav = slider.querySelector('[data-slider-nav]'); // Slider nav wrapper


				// METHODS

				// Display count of slides
				var createSlideCount = function () {
					var slideTotal = mySwipe[index].getNumSlides();
					var slideCurrent = mySwipe[index].getPos();
					if ( slideCount !== null ) {
						slideCount.innerHTML = slideCurrent + ' of ' + slideTotal;
					}
				};

				// Display Slider navigation
				var createNavButtons = function () {
					if ( slideNav !== null ) {
						slideNav.innerHTML = '<a data-slider-nav-prev href="#">Previous</a> | <a data-slider-nav-next href="#">Next</a>';
					}
				};

				// Stop YouTube, Vimeo, and HTML5 videos from playing when leaving the slide
				var stopVideo = function () {
					var currentSlide = mySwipe[index].getPos() - 1;
					var iframe = slider.querySelector( '[data-index="' + currentSlide + '"] iframe');
					var video = slider.querySelector( '[data-index="' + currentSlide + '"] video' );
					if ( iframe !== null ) {
						var iframeSrc = iframe.src;
						iframe.src = iframeSrc;
					}
					if ( video !== null ) {
						video.pause();
					}
				};

				// Handle next button
				var handleNextBtn = function (event) {
					event.preventDefault();
					stopVideo();
					mySwipe[index].next();
				};

				// Handle previous button
				var handlePrevBtn = function (event) {
					event.preventDefault();
					stopVideo();
					mySwipe[index].prev();
				};

				// Handle keypress
				var handleKeypress = function (event) {
					if ( event.keyCode == 37 ) {
						mySwipe[index].prev();
					}
					if ( event.keyCode == 39) {
						mySwipe[index].next();
					}
				};


				// EVENTS, LISTENERS, AND INITS

				// Activate Slider
				mySwipe[index] = Swipe(slider, {
					// startSlide: 2,
					// speed: 400,
					// auto: 3000,
					continuous: true,
					// disableScroll: false,
					// stopPropagation: false,
					callback: function(index, elem) {
						createSlideCount(); // Update with new position on slide change
					},
					// transitionEnd: function(index, elem) {}
				});

				// Create slide count and nav
				createSlideCount();
				createNavButtons();
				var btnNext = slider.querySelector('[data-slider-nav-next]'); // Next slide button
				var btnPrev = slider.querySelector('[data-slider-nav-prev]'); // Previous slide button

				// Toggle Previous & Next Buttons
				if ( btnNext ) {
					btnNext.addEventListener('click', handleNextBtn, false);
				}
				if ( btnPrev ) {
					btnPrev.addEventListener('click', handlePrevBtn, false);
				}

				// Toggle Left & Right Keypress
				window.addEventListener('keydown', handleKeypress, false);

			});

		}

	})(window, document);
</script>

<details>
  <summary><h2>The title of the image group</h2></summary>
  <img src="https://images.gr-assets.com/authors/1335187160p7/23906.jpg" alt="image-description"/>
  <img src="https://images.gr-assets.com/authors/1335187160p7/23906.jpg" alt="image-description"/>
  <img src="https://images.gr-assets.com/authors/1335187160p7/23906.jpg" alt="image-description"/>
</details>

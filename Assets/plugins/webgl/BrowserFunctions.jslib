

  var BrowserFunctions = {
	  RequestFullScreen: function () {
	      if (!document.fullscreenElement) {
			const unityCanvas = document.querySelector("#unity-canvas");
			document.querySelector("#unity-container").style.background = "black";
		
			setTimeout(() => {
					unityCanvas.style.display = "none";
			}, 200)

			function createButton() {
				var button = document.createElement("button");
				button.textContent = "This game only works with fullscreen. Ok? Click here";
				button.style.position = "absolute";
				button.style.top = "50%";
				button.style.left = "50%";
				button.style.transform = "translate(-50%, -50%)";
				button.onclick = () => {
					button.textContent = "Click again";
					document.onpointerup = () => {
						getFullScreen();
					}
					if (document.fullscreenElement) {
						hideButton();
					}
				}
				return button;
			  }
          
			  var button = createButton();
			  document.body.appendChild(button);

			function getFullScreen () {
					console.log('js requestfullscreen');
					const elem = document.body;
					if (elem.requestFullscreen) {
					elem.requestFullscreen();
					} else if (elem.webkitRequestFullScreen) {
					elem.webkitRequestFullScreen();
					} else if (elem.mozRequestFullScreen) {
					elem.mozRequestFullScreen();
					} else if (elem.msRequestFullscreen) {
					elem.msRequestFullscreen();
					}
				let counter = 0;
				const i = setInterval(() => {
					counter++;
					if (document.fullscreenElement) {
						screen.orientation.lock('landscape-primary');
						unityCanvas.style.display = "";
						resetAllEvents();
						clearInterval(i);
					} else {
						button.textContent = "try again";
					}
					if (counter >= 100) {
						clearInterval(i);
					}
				}, 10)
			}
			function resetAllEvents() {
				hideButton();
				button.onclick = null;
				document.onpointerup = null;
			}
			function hideButton() {
				button.style.display = "none";
			}
		  }
	},
	ExitFullScreen: function() {
		document.exitFullscreen();
	},
};

mergeInto(LibraryManager.library, BrowserFunctions);
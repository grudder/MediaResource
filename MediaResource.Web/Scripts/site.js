// iframe高度自适应
function iframeAutoHeight() {
	var iframeId = event.srcElement.id;
	var ifm = document.getElementById(iframeId);
	var subWeb = document.frames ? document.frames[iframeId].document : ifm.contentDocument;
	if (ifm != null && subWeb != null) {
		ifm.height = subWeb.body.scrollHeight + 40;
	}
}

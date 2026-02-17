mergedInto(LibraryManager.library, {
  IsMobile: function () {
    var userAgent = window.navigator.userAgent.toLowerCase();
    var mobile = /android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini/i.test(userAgent);
    return mobile;
  }
});

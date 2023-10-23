

class VideoHelpers {
    static dotNetHelper;

    static setDotNetHelper(value) {
        VideoHelpers.dotNetHelper = value;

    }

    static async videoEnded() {
        await VideoHelpers.dotNetHelper.invokeMethodAsync('VideoEnded');
    }

}
window.VideoHelpers = VideoHelpers;

window.addVideoEndedEventListener = (videoId) =>
{
    var video = document.getElementById(videoId);
    video.addEventListener("ended", async function () {
        // Perform any action you want when the video has ended
        await VideoHelpers.videoEnded();
    });
}
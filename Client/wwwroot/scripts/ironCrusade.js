"use strict";

window.ironCrusade = window.ironCrusade || {};

ironCrusade.say = function (message) {
    const msg = new SpeechSynthesisUtterance();
    msg.text = message;
    msg.lang = "nl-NL";
    window.speechSynthesis.speak(msg);
};
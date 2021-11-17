const _console = document.getElementsByClassName("gameConsole");

export const cmd = {
    DisplayMessage: function (message) {
        if (message.length < 1) {
            return;
        }
        if (_console[0].value == "") {
            _console[0].value = message;
        }
        else {

            _console[0].value = _console[0].value + "\n" + message;
        }
    },

    Clear: function() {
        _console[0].value = "";
    }
}
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
            _console[0].value = _console[0].value + "\n";
            let wordsToDisplay = message.split(" ");
            wordsToDisplay.forEach(function (word, index) {
                setTimeout(function() {
                    _console[0].value = _console[0].value + word + " "
                }, index * 50);
                });
            _console[0].scrollTop = _console[0].scrollHeight;
        }
        
    },

    Clear: function() {
        _console[0].value = "";
        _console[0].scrollTop = _console[0].scrollHeight;
    }
}
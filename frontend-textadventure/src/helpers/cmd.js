const _console = document.getElementsByClassName("Console");

String.prototype.destructiveInsert = function (index, string) {
    return this.substring(0, index) + this.substring(index).replace(" ".repeat(string.length), string);
};

String.prototype.insert = function (index, string) {
    if (index > 0) {
        return this.substring(0, index) + string + this.substr(index);
    }
    return string + this;
};
export function UseCMDWrapper() {
    return {
        display: DisplayMessage(),
        clear: Clear()
    }
    
    function DisplayMessage() {
        return (message) => {
            if (_console[0] == undefined || message.length < 1) {
                return;
            }
            else {
                //setup the line that will be written to
                if (_console[0].value !== "") {
                    _console[0].value = _console[0].value + "\n";
                }
                let startOfLine = _console[0].value.length;
                _console[0].value = _console[0].value.insert(startOfLine, " ".repeat(message.length + 1));
                
                let wordsToDisplay = message.split(" ");
                wordsToDisplay.forEach(function (word, index) {
                    setTimeout(function () {
                        _console[0].value = _console[0].value.destructiveInsert(startOfLine, word + " ");
                        startOfLine += (word.length + 1);
                    }, index * 50);
                });
                _console[0].scrollTop = _console[0].scrollHeight;
            }
        }
    }
    
    
    function Clear() {
        return () => {
            if (_console[0] == undefined) {
                return;
            }
            _console[0].value = "";
            _console[0].scrollTop = _console[0].scrollHeight;
        }
    }
};
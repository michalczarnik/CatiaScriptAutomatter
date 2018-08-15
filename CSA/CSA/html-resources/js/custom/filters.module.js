app.filter("returnName", function () {
    return function (path) {
        let myRegex = /(.*\\.*\\)(.*)/;
        let match = myRegex.exec(path);
        if (match && match.length > 2)
            return match[2];
        return "";
    }
});
app.service('macroService', function () {
    this.getAllMacros = function (path) {
        return JSON.parse(exposedClass.loadMacros(path));
    };

    this.runMacro = function (uniqueId, parameters) {
        var responseModel = createResponseModel(uniqueId, parameters);
        exposedClass.runMacro(JSON.stringify(responseModel));
    };

    this.checkIfCatiaIsOnline = function () {
        return exposedClass.isCatiaStarted();
    };

    this.loadRepositories = function () {
        return JSON.parse(exposedClass.loadRepositories());
    };

    this.addRepository = function () {
        let path = exposedClass.getFolderPath();
        return exposedClass.addRepository(path);
    };

    this.deleteRepository = function (path) {
        return exposedClass.deleteRepository(path);
    };

    this.changeToDefault = function (path) {
        return exposedClass.changeToDefault(path);
    };

    this.getFolderPath = function () {
        return exposedClass.getFolderPath();
    };

    function createResponseModel(uniqueId, parameters) {
        var object = {
            UniqueID:uniqueId,
            ParameterList:[]
        };
        for (var i = 0; i < parameters.length; i++){
            var parameter = {
                ParamterName: parameters[i].ParameterName,
                ParameterValue: parameters[i].DefaultValue
            };
            object.ParameterList.push(parameter);
        }
        return object;
    };
});
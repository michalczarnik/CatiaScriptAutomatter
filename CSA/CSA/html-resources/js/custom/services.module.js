app.service('macroService', function () {
    this.getAllMacros = function () {
        return JSON.parse(exposedClass.loadMacros());
    }

    this.runMacro = function (uniqueId, parameters) {
        var responseModel = createResponseModel(uniqueId, parameters);
        exposedClass.runMacro(JSON.stringify(responseModel));
    }

    this.checkIfCatiaIsOnline = function(){
        return exposedClass.isCatiaStarted();
    }

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
    }
});
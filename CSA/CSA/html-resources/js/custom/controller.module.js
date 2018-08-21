app.controller("pluginController", function ($scope, $interval, macroService) {
    $scope.firstName = "johnyy";
    $scope.lastName = "Labida";
    $scope.macros = [];
    $scope.catiaStarted = false;
    $scope.selectedMacro = "";
    $scope.isRunAll = false;
    $scope.currentMacro = "";
    $scope.macrosQueue = [];
    $scope.errorMessage = "";
    $scope.isWatchOperationInProgress = false;
    $scope.sharedVaraiables = {};

    $scope.init = function () {
        $interval(function () {
            $scope.catiaStarted = macroService.checkIfCatiaIsOnline();
        }, 1000);
    }

    $scope.selectMacroInRunAll = function (macro) {
        if (macro.isSelected == true) {
            macro.isSelected = false;
        } else {
            macro.isSelected = true;
            let first = -1;
            let last = -1;
            for (var i = 0; i < $scope.macros.length; i++) {
                if ($scope.macros[i].isSelected == true) {
                    if (first > i || first==-1)
                        first = i;
                    if (last < i || last == -1)
                        last = i;
                }
            }
            for (var i = first; i <= last; i++) {
                $scope.macros[i].isSelected = true;
            }
        }
    }

    $scope.startRunSelected = function (isFirstRun) {
        if (isFirstRun) {
            $scope.macrosQueue = [];
            for (let i = 0; i < $scope.macros.length; i++) {
                if ($scope.macros[i].isSelected)
                    $scope.macrosQueue.push(i);
            }
        }
        if ($scope.macrosQueue.length > 0) {
            $scope.isRunSelected = true;
            let currMacro = $scope.macrosQueue.shift();
            if ($scope.macros[currMacro].Warnings !== null && $scope.macros[currMacro].Warnings.IsAfter==false) {
                $("#warning" + $scope.macros[currMacro].UniqueID).modal();
            } else if ($scope.macros[currMacro].Warnings !== null && $scope.macros[currMacro].Warnings.IsAfter == true){
                let response = macroService.runMacro($scope.macros[currMacro].UniqueID, $scope.macros[currMacro].ParameterList);
                $($scope.macros[currMacro].UniqueID + "input").collapse();
                if (response) {
                    $scope.showError(response);
                    return;
                }
                $("#warning" + $scope.macros[currMacro].UniqueID).modal();
            }else {
                let response = macroService.runMacro($scope.macros[currMacro].UniqueID, $scope.macros[currMacro].ParameterList);
                $($scope.macros[currMacro].UniqueID+"input").collapse();
                if (response) {
                    $scope.showError(response);
                    return;
                }
                $scope.startRunSelected(false);
            }
        } else {
            $scope.isRunSelected = false;
            $("#runSelected").modal("hide");
            alert("Wszystkie wybrane skrypty zakońćzone!");
        }
    }

    $scope.continueRunSelected = function (index) {
        var macro = $scope.macros[index];
        if (macro.Warnings.IsAfter != true) {
            let response = macroService.runMacro(macro.UniqueID, macro.ParameterList);
            $(macro.UniqueID + "input").collapse();
            if (response) {
                $scope.showError(response);
                return;
            }
        }
        $("#warning" + $scope.macros[index].UniqueID).modal("hide");
        $scope.startRunSelected(false);
    }

    $scope.showRunSelectedModal = function () {
        $("#runSelected").modal();
    }

    $scope.runAll = function (index) {
        if (index < $scope.macros.length && index >= 0) {
            $scope.isRunAll = true;
            $scope.currentMacro = $scope.macros[index].UniqueID;
            if ($scope.macros[index].Warnings !== null && $scope.macros[index].Warnings.IsAfter == false) {
                $("#warning" + $scope.macros[index].UniqueID).modal();
            } else if ($scope.macros[index].Warnings !== null && $scope.macros[index].Warnings.IsAfter == true) {
                let response = macroService.runMacro($scope.macros[index].UniqueID, $scope.macros[index].ParameterList);
                $($scope.macros[index].UniqueID + "input").collapse();
                if (response) {
                    $scope.showError(response);
                    return;
                }
                $("#warning" + $scope.macros[index].UniqueID).modal();
            } else {
                let response = macroService.runMacro($scope.macros[index].UniqueID, $scope.macros[index].ParameterList);
                $($scope.macros[index].UniqueID + "input").collapse();
                if (response) {
                    $scope.showError(response);
                    return;
                }
                $scope.runAll(index + 1);
            }
        } else {
            $scope.isRunAll = false;
            $scope.currentMacro = "";
            alert("Wszystkie skrypty zakończone!");
        }
    }

    $scope.runAllModal = function (index) {
        if (index < $scope.macros.length && index >= 0) {
            var macro = $scope.macros[index];
            if (macro.Warnings.IsAfter != true) {
                let response = macroService.runMacro($scope.macros[index].UniqueID, $scope.macros[index].ParameterList);
                $($scope.macros[index].UniqueID + "input").collapse();
                if (response) {
                    $scope.showError(response);
                    return;
                }
            }
            $("#warning" + $scope.macros[index].UniqueID).modal("hide");
            $scope.runAll(index + 1);
        }
        else {
            $scope.isRunAll = false;
            $("#warning" + $scope.macros[index].UniqueID).modal("hide");
            $scope.currentMacro = "";
        }
    }

    $scope.loadMacros = function () {
        $scope.macros = macroService.getAllMacros();
        $scope.populateImageModel();
        $scope.populateWatches();
    }

    $scope.populateWatches = function () {
        for (let i = 0; i < $scope.macros.length; i++) {
            for (let j = 0; j < $scope.macros[i].ParameterList.length; j++) {
                var parameter = $scope.macros[i].ParameterList[j];
                if (parameter.IsSharedInContext) {
                    if (!$scope.sharedVaraiables[parameter.ParameterName]) 
                        $scope.sharedVaraiables[parameter.ParameterName] = [];
                    $scope.sharedVaraiables[parameter.ParameterName].push({ macroIndex: i, parameterIndex: j });
                    $scope.$watch('macros[' + i + '].ParameterList[' + j + ']', function (newVal, oldVal) {
                        if (!$scope.isWatchOperationInProgress && newVal && newVal.DefaultValue !== oldVal.DefaultValue) {
                            $scope.isWatchOperationInProgress = true;
                            let name = newVal.ParameterName;
                            let newValue = newVal.DefaultValue;
                            let parameters = $scope.sharedVaraiables[name];
                            for (let k = 0; k < parameters.length; k++) {
                                $scope.macros[parameters[k].macroIndex].ParameterList[parameters[k].parameterIndex].DefaultValue = newValue;
                            }
                            $scope.isWatchOperationInProgres = false;
                        }
                    }, true);
                }
            }
        }        
    }

    $scope.selectMacro = function (id) {
        $scope.selectedMacro = id;
        $(".collapse.show").collapse('hide');
    }

    $scope.showModal = function (macro) {
        if (macro.Warnings!=null && macro.Warnings.IsAfter) {
            let response = macroService.runMacro(macro.UniqueID, macro.ParameterList);
            if (response) {
                $scope.showError(response);
                return;
            }
        }
        $("#warning" + macro.UniqueID).modal();
        if (macro.Warnings !=null && macro.Warnings.IsAfter) 
            alert("Wybrany skrypt zakończony!");

    }

    $scope.populateImageModel = function () {
        $scope.macros.forEach(function (macro) {
            macro.ImagesModel = [];
            var keys = Object.keys(macro.Images);
            keys.forEach(function (key) {
                var path = macro.Images[key];
                macro.ImagesModel.push({ Name: key, Path: path });
            })
        });
    }

    $scope.runMacro = function (macroInformation) {
        if (macroInformation.Warnings==null || !macroInformation.Warnings.IsAfter) {
            let response = macroService.runMacro(macroInformation.UniqueID, macroInformation.ParameterList);
            if (response) {
                $scope.showError(response);
                return;
            }
            if (macroInformation.Warnings==null)
                alert("Wybrany skrypt zakończony!");        }
    }

    $scope.selectFile = function (macroIndex, paramIndex) {
        var path = exposedClass.openFile();
        $scope.macros[macroIndex].ParameterList[paramIndex].DefaultValue = path;
    }

    $scope.showError = function (message) {
        $(".warning-modal").modal("hide");
        $("#runSelected").modal("hide");
        $scope.isRunAll = false;
        $scope.currentMacro = "";
        $scope.isRunSelected = false;
        $scope.errorMessage = message;
        $("#errorMessage").modal(); 
    }

    $scope.closeErrorModal = function () {
        $("#errorMessage").modal("hide"); 
    }
});
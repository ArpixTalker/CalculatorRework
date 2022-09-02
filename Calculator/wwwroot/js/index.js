$(document).ready(function () {

    const _operation_equal = "=";
    const _operation_clear = "C";
    const _operatorArray = ["+","-","/","*"]
    let _display = $("#calc_display");
    let _displayingResult = false;
    let _expression = "";
    refreshHistory();

    $('.calc_element').click(function () {

        let operation = $(this).attr("operation");

        //Check if user can continue with result
        if (!_operatorArray.includes(operation.trim()) && _displayingResult) {
            _expression = "";
            _displayingResult = false;
        }
        else {
            _displayingResult = false
        }

        //Check if expression should be evaluated
        if (operation === _operation_equal) {

            getResult(_expression)
            return;
        }

        //Check if display shold be cleared
        if (operation === _operation_clear) {
            _expression = "";
            _display.val("");
            return;
        }

        //add character and display expression
        _expression = _expression + operation;
        _display.val(_expression);

    })

    //Calls an AJAX to obtain either result of the expression or error message
    function getResult(expression) {

        const whole_numbers = $('#whole_numbers_checkbox').is(":checked");

        $.ajax({

            method: "POST",
            async: false,
            url: "api/v1/math/compute",
            data: {

                expression: expression,
                whole: whole_numbers

            }, success: function (output) {

                if (output === "error") {
                    showError();
                }
                else
                {
                    _display.val(output);
                    _displayingResult = true;
                    _expression = output;
                    refreshHistory();
                }
                    
            }
        });
    }

    //AJAX call to get latest ten expressions
    function refreshHistory()
    {
        let div = $("#history_content");
        $.ajax({

            method: "GET",
            async: false,
            url: "api/v1/math/getLatestExpressions",
            success: function (response)
            {
                let appendString = "";
                response.forEach(function (element) {
                    appendString += "<div class=\"row\"> <p>" + element.expression + "</p></div>";
                })        
                div.html(appendString);
            }
        })
        
    }

    //Displays error
    function showError()
    {
        _display.val("_ERR")
        _expression = "";
    }
});
﻿var suggest_count = 0;
var input_initial_value = "";
var suggest_selected = 0;

var key_activate = (n) => {
    console.log("I'm here");
    $("#search_advice_wrapper div").eq(suggest_selected - 1).removeClass("active");

    if (n === 1 && suggest_selected < suggest_count) {
        suggest_selected++;
    } else if (n === -1 && suggest_selected > 0) {
        suggest_selected--;
    }

    if (suggest_selected > 0) {
        $("#search_advice_wrapper div").eq(suggest_selected - 1).addClass("active");
        $("#search_box").val($("#search_advice_wrapper div").eq(suggest_selected - 1).text());
    } else {
        $("#search_box").val(input_initial_value);
    }
};

var chooseIt = (str, i) => {
    document.getElementById("search_box").value = str;
    $("#search_advice_wrapper").fadeOut(350).html("");
    document.getElementById("questionIdNew").value = i;
};


$("#search_box").keypress(i => {
    $.ajax({
        url: "/survey/LoadUnique",
        method: "POST",
        data: JSON.stringify({
            Question: $("#search_box").val() + i.key,
            TestPool: $('#testPoolId').text()
        }),
        contentType: "application/json; charset=utf-8",
        success: data => {
            var suggest_count = data.length;
            if (suggest_count > 0) {
                $("#search_advice_wrapper").html("").show();
                for (var i in data) {
                    if (data[i] !== "") {
                        var str = data[i]["Item1"];
                        $("#search_advice_wrapper").append("<div onclick=\"chooseIt('" + str + "'," + data[i]["Item2"] + ");\" class=\"advice_variant\" id=\"" + data[i]["Item2"] + "\">" + data[i]["Item1"] + "</div>");
                    }
                }
            }
        }
    });
});
$("#search_box").keydown(I => {
    switch (I.keyCode) {
    case 13: // enter
    case 27: // escape
        $("#search_advice_wrapper").hide();
        return false;
        break;
    case 38: // стрелка вверх
    case 40: // стрелка вниз
        I.preventDefault();
        if (suggest_count) {
            key_activate(I.keyCode - 39);
        }
        break;
    }
});


$("html").click(() => {
    $("#search_advice_wrapper").hide();
});

$("#search_box").click(event => {
    if (suggest_count)
        $("#search_advice_wrapper").show();
    event.stopPropagation();
});

var checkIt = (first, second) => {
    return first === second;
};
var addTestPoolEntry = () => {
    $.ajax({
        url: "/testpoolentry/Create",
        method: "POST",
        data: JSON.stringify({
            Id: 0,
            Score: parseInt($("#scoreNew").val()),
            ScoringStrategy: parseInt($("#selectNew option:selected").val()),
            TestPool: parseInt($("#testPoolId").text()),
            TestQuestion: parseInt($("#questionIdNew").val())
        }),
        contentType: "application/json; charset=utf-8",
        success: data => {
            if (data !== false) {
                if (checkIt("У этого тестпула ещё нет добавленных вопросов.", $("#editorRows").find("p").text())) {
                    $("#editorRows").html("");
                }
                $("#editorRows").append("" +
                    "<div class=\"row\" id=\"testPoolEntry_" + data + "\">" +
                    "<div class=\"itemId\" hidden=\"true\">" + data + "</div>" +
                    "<div class=\"col-md-3\">" +
                    "<p>Выбранный вопрос</p>" +
                    "<p id=\"question_" + data + "\">" + $("#search_box").val() + "</p>" +
                    "</div>" +
                    "<div class=\"col-md-3\">" +
                    "<p>Количество баллов</p>" +
                    "<input id=\"score_" + data + "\" type=\"number\" value=\"" + $("#scoreNew").val() + "\" onblur=\"editTestPoolEntryScore(" + data + ");\"/>" +
                    "</div>" +
                    "<div class=\"col-md-3\">" +
                    "<div class=\"editor-label\">Выбор стратегии</div>" +
                    "<select id=\"select_" + data + "\" onblur=\"editTestPoolEntrySelect(" + data + ");\">" +
                    "<option selected=\"" + checkIt($("#selectNew option:selected").val(), "Учёт только верных") + "\" value=\"0\">Учёт только верных</option>" +
                    "<option selected=\"" + checkIt($("#selectNew option:selected").val(), "Учёт всех") + "\" value=\"1\">Учёт всех</option>" +
                    "</select>" +
                    "</div>" +
                    "<div class=\"col-md-3\">" +
                    "<button onclick=\"deleteTestPoolEntry(" + data + ");\" type=\"button\" id=\"delete_" + data + "\">Удалить</button>" +
                    "</div>" +
                    "</div>");
                $("#scoreNew").val("");
                $("#testPoolId").val("");
                $("#questionIdNew").val("");
                $("#search_box").val("");
            } else {
                $("#editorRows").prepend("<div class=\"alert alert-danger\" role=\"alert\">" +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>" +
                    "Добавление прошло неуспешно, проверьте данные!</div>");
            }
        },
        error: data => {
            $("#editorRows").prepend("<div class=\"alert alert-danger\" role=\"alert\">" +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>" +
                    "Нельзя добавить такие данные!</div>");
        }
    });
};

var deleteTestPoolEntry = (data) => {
    $.ajax({
        url: "/testpoolentry/Delete",
        method: "POST",
        data: JSON.stringify({
            Id: parseInt(data),
            Score: 0,
            ScoringStrategy: 0,
            TestPool: 0,
            TestQuestion: 0
        }),
        contentType: "application/json; charset=utf-8",
        success: answer => {
            if (answer) {
                $("#testPoolEntry_" + data).text("");
                if ($("#editorRows").text().trim() === "") {
                    $("#editorRows").html("<p>У этого тестпула ещё нет добавленных вопросов.</p>");
                }
            } else {
                $("#editorRows").prepend("<div class=\"alert alert-danger\" role=\"alert\">" +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>" +
                    "Невозможно удалить этот элемент!</div>");
            }
        }
    });
};


var editTestPoolEntrySelect = (data) => {
    var value = document.getElementById("select_" + data).value;
    $.ajax({
        url: "/testpoolentry/Edit",
        method: "POST",
        data: JSON.stringify({
            Id: parseInt(data),
            Type: "ScoringStrategy",
            Value: value
        }),
        contentType: "application/json; charset=utf-8",
        success: answer => {
            if (answer) {
                $("#editorRows").prepend("<div class=\"alert alert-success\" role=\"alert\">" +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>" +
                    "Значение успешно изменилось!</div>");
            } else {
                $("#editorRows").prepend("<div class=\"alert alert-danger\" role=\"alert\">" +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>" +
                    "Значение не изменилось, повторите позже!</div>");
            }
        }
    });
};

var editTestPoolEntryScore = (data) => {
    var value = document.getElementById("score_" + data).value;
    $.ajax({
        url: "/testpoolentry/Edit",
        method: "POST",
        data: JSON.stringify({
            Id: parseInt(data),
            Type: "Score",
            Value: value
        }),
        contentType: "application/json; charset=utf-8",
        success: answer => {
            if (answer) {
                $("#editorRows").prepend("<div class=\"alert alert-success\" role=\"alert\">" +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>" +
                    "Значение успешно изменилось!</div>");
            } else {
                $("#editorRows").prepend("<div class=\"alert alert-danger\" role=\"alert\">" +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>" +
                    "Значение не изменилось, повторите позже!</div>");
            }
        }
    });
};
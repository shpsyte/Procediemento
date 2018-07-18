function GetDataJason(url, controle) {
    $.getJSON(url, GetData);



    function GetData(json) {

        //Limpar os itens que são maiores que 0
        //Ou seja: não retirar o primeiro item
        $(controle + " :gt(0)").remove();


        $(json).each(function () {
            //adicionando as opções de acordo com o retorno
            $(controle).append("<option value='" + this.cd_value + "'>" + this.texto + "</option>");

        });
    }

}

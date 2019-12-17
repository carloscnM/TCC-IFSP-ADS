/*---------- Adicionar colaborador -------------*/

function submitform() {
    document.forms["CadastrarCola"].submit();
};

/*---------- Fim Adicionar colaborador -------------*/

/*---------- Nova Analise -------------*/



/*---------- Fim Nova Analise -------------*/

/*---------- SelExperimenrto -------------*/




/*---------- Fim  SelExperimenrto -------------*/





/* Selecionar Experimento Consulta*/
 
 


 /* Fim Selecionar Experimento Consulta*/






/*---------- CadastrarGrupo -------------*/




/*---------- Fim CadastrarGrupo-------------*/



/*---------- AddSujeitoExperimento -------------*/




/*---------- Fim AddSujeitoExperimento -------------*/

/*---------- CaptarEditarDados -------------*/


function ttint(e) {
    debugger;
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
};



function ttfloat(e) {
    debugger;
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 43 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
};



/*---------- Fim CaptarEditarDados -------------*/

/*---------- CaptarEditarDadosExperimento -------------*/






/*---------- CaptarEditarDadosExperimento -------------*/


/*---------- CriarProjeto -------------*/



/*---------- Fim CriarProjeto -------------*/


/*---------- EditarProjeto -------------*/





/*---------- Fim EditarProjeto -------------*/


var TipoAcesso;

function guardaTipoAcesso(tipoacesso) {
    tipoacesso = tipoacesso;
};

function obterTipoAcesso() {
    return TipoAcesso;
}
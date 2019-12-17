/*-------------- Projeto em Andamento ------------------*/


function NovoProjeto() {
    $("#modalCadastro").load("/Projetos/CriarProjeto", function () {
        $("#cadastrarProjeto").modal();
    })
};



function edit(id) {
    $("#modal").load("/Projetos/EditarProjeto?Id=" + id, function () {
        $("#editarProjeto").modal();
    })
};


function DeletarProjeto(id) {
    $("#modalExclusao").load("/Projetos/ExcluirProjeto?Id=" + id, function () {
        $("#exprojeto").modal();
    })
};


/*-------------- Fim Projeto em Andamento ------------------*/

/*-------------- Acessar Projeto ------------------*/



function listarColaborador(id) {
    $("#modalColaborador").load("/Projetos/ColaboradoresDaAnalise?IdAnalise=" + id, function () {
        $("#listarCola").modal();
    })
};



function NovaAnalise(id) {
    $("#adicionarAna").load("/Analise/NovaAnalise?IdProjeto=" + id, function () {
        $("#addAnalise").modal();
    })
};


/*-------------- Fim Acessar Projeto ------------------*/

/*--------------  Acessar Analise ------------*/

function FSelAcaoExpSuj(IdAnalise, IdProjeto, IdSujeito) {
    $("#SelAcaoExpSuj").load("/Sujeito/ListarExperimentoSujeito?IdAnalise=" + IdAnalise + "&IdProjeto=" + IdProjeto + "&IdSujeito=" + IdSujeito, function () {
        $("#ModSelExpAcao").modal();
    })
};



function FSelExperimento(IdAnalise, IdProjeto) {
    $("#SelExperimento").load("/Experimento/SelExperimento?IdAnalise=" + IdAnalise + "&IdProjeto=" + IdProjeto, function () {
        $("#ModSelExperimento").modal();
    })
};




function cadCaracSujeito(IdAnalise, IdProjeto) {
    $("#DivModalCadCaraSuj").load("/Sujeito/CadastrarCaraSujeito?IdAnalise=" + IdAnalise + "&IdProjeto=" + IdProjeto, function () {
        $("#CadCaraSujeitos").modal();
    })
};




function NovoSujeito(IdAnalise, IdProjeto) {
    $("#NovoSujeitoParaAnalise").load("/Sujeito/NovoSujeito?IdAnalise=" + IdAnalise + "&IdProjeto=" + IdProjeto, function () {
        $("#modalCadastroSujeito").modal();
    })
};




function CadastrarGrupo(IdAnalise, IdProjeto) {
    $("#ModalCadastraGrupoDiv").load("/Grupo/CadastrarGrupo?IdAnalise=" + IdAnalise + "&IdProjeto=" + IdProjeto, function () {
        $("#modalCadastroGrupo").modal();
    })
};



function VisualizarSujeitos(IdSujeito) {
    $("#VisualizarCaracteriscas").load("/Sujeito/CaracteristicasComuns?IdSujeito=" + IdSujeito, function () {
        $("#VisualizarCaraComuns").modal();
    })
};



function CaptarEditarDados(IdSujeito, IdAnalise, IdProjeto) {
    $("#CaptarEditarDadosDiv").load("/Medicao/CaptarEditarDados?IdSujeito=" + IdSujeito + "&IdAnalise=" + IdAnalise + "&IdProjeto=" + IdProjeto, function () {
        $("#CaptarEditarDados").modal();
    })
};







/*-------------- Fim Acessar Analise ------------------*/

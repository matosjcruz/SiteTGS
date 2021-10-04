
$(function () {

	$('.carousel').slick({
		dots: false,
		infinite: true,
		slidesToShow: 3,
		slidesToScroll: 1
	});

	$('.item_modulo').click(function () {
		var id = $(this).data('id');
		if (id > 0) {
			$('.item_modulo').removeClass('submenu_ativo');
			$(this).addClass('submenu_ativo');
			$('.conteudo_modulo').addClass('hidden');
			$('.conteudo_modulo').each(function (a, b) {
				if ($(b).data('id') == id) {
					$(b).removeClass('hidden');
				}
			});
		}
	});
	$("#enviarContato").click(function (event) {
		var vnome, vtelefone, vemails, vmensagem;
		vnome = $("#contatoNome").val();
		vtelefone = $("#contatoTelefone").val();
		vemails = $("#contatoEmail").val();
		vmensagem = $("#contatoMensagem").val();

		$.ajax({
			type: "POST",
			url: "/Home/SendMail",
			data: { Nome: vnome, Telefone: vtelefone, Email: vemails, Mensagem: vmensagem },
			success: function (d) {
				alert('Email enviado com sucesso!');
			},
			error: function (d) {
			}
		});

		event.preventDefault();
	});
	$("#btnLogin").click(function (event) {
		var vLogin, vSenha;
		vLogin = $("#login").val();
		vSenha = $("#senha").val();

		$.ajax({
			type: "POST",
			url: "/Home/ValidaLogin",
			data: { Login: vLogin, Senha: vSenha },
			success: function (d) {
				console.log(d);
			},
			error: function (d) {
				console.log(d);
			}
		});

		event.preventDefault();
	});
});


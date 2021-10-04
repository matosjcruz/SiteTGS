function humanFileSize(bytes, si = false, dp = 1) {
	const thresh = si ? 1000 : 1024;

	if (Math.abs(bytes) < thresh) {
		return bytes + ' B';
	}

	const units = si
		? ['kB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB']
		: ['KiB', 'MiB', 'GiB', 'TiB', 'PiB', 'EiB', 'ZiB', 'YiB'];
	let u = -1;
	const r = 10 ** dp;

	do {
		bytes /= thresh;
		++u;
	} while (Math.round(Math.abs(bytes) * r) / r >= thresh && u < units.length - 1);


	return bytes.toFixed(dp) + ' ' + units[u];
}


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
			error: function (xhr, ajaxOptions, thrownError) {
				if (xhr.status == 500) {
					alert('Ocorreu um erro no envio do email');
				}
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
			success: function (ret) {
				var obj = JSON.parse(ret);
				for (const [key, value] of Object.entries(obj)) {
					var objArq = (key, value);
					var textoHTML = "<tr><td>" + objArq.Nome + "</td><td>" + objArq.Tamanho
					textoHTML += "</td><td>" + objArq.Data + "</td><td><a href='/Home/DownloadFile/" + objArq.Nome+"' class='btnDownload'><img src='/img/ic_download.png' class='botao_Download'></a></td></tr>"
					$('#gvDownloads > tbody:last-child').append(textoHTML);
					$('.dvDownloads').removeClass("hidden");
					$('.formLogin').addClass("hidden");
				}
			},
			error: function (xhr, ajaxOptions, thrownError) {
				if (xhr.status == 500) {
					alert('Login e senha incorretos');
					$("#login").focus();
				}
			}
		});
		event.preventDefault();
	});
	
});



# FSharpSocket


Guia de Instalação da linguagem para linux:
http://fsharp.org/use/linux/
(utilizada opção 1:  F# Debian/Ubuntu packages)

Guia para desenvolvimento da linguagem em linux:
http://fsharp.org/guides/mac-linux-cross-platform/
(a parte Command Line Tools é extremamente útil)

Referência Rápida da linguagem:
http://dungpa.github.io/fsharp-cheatsheet/
(Excelente para aprender as estruturas básicas em poucos minutos)

Tutorial interativo de F#:
http://www.tryfsharp.org/Learn/getting-started
(Não tive paciência pra fazer ele todo, mas já deu pra pegar as idéias principais)


Aprendendo Sockets:
https://en.wikipedia.org/wiki/Network_socket
http://www.linuxhowtos.org/C_C++/socket.htm
http://www.tutorialspoint.com/unix_sockets/what_is_socket.htm
(Apenas para ter conhecimento de como implementar um Socket)


Códigos de exemplo:

https://gist.github.com/panesofglass/765088
(Exemplo de um Servidor com socket em F# - Utilizei parte do código como base para ambas as partes)port

http://zguide.zeromq.org/fsx:asyncsrv
(Exemplo de comunicação entre cliente e servidor)

https://social.msdn.microsoft.com/Forums/vstudio/en-US/0378c5eb-1588-4102-9afa-4bde8d9cc2c1/f-read-data-from-tcp-socket-stream?forum=fsharpgeneral
(Thread de fórum onde resolvi o problema da quantidade de bits exceder o tamanho da transferência)



















1) Primeiramente, FORA TEMER!

2) Instale o F# e o Mono no seu linux.
Para distros debian-based: 

sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list

sudo apt-get update

echo "deb http://download.mono-project.com/repo/debian wheezy-apache24-compat main" | sudo tee -a /etc/apt/sources.list.d/mono-xamarin.list

 sudo apt-get update
 sudo apt-get install mono-complete fsharp

Para outras distribuições: http://fsharp.org/use/linux/

3) Só para ter certeza que tudo foi instalado corretamente, abra o interpretador de F# no terminal, digitando o comando:
fsharpi

Se você conseguir entrar com o comando 
1+1;;
e a resposta for 

val it : int = 2

deu tudo certo !


4) para executar, (assumindo que o código servidor é Server.fsx e o código cliente é Client.fsx) entre com o comando

fsharpi Server.fsx pasta/onde/salvar/o/arquivo porta

ou apenas
fsharpi Server.fsx
(dessa forma, ele irá utilizar a porta 8593 e salvar na pasta /tmp)
(Esqueci de implementar a mudança de porta no cliente, de qualquer forma, então vai ter de ser essa porta mesmo)

Para executar o cliente:
fsharpi /caminho/absoluto/do/arquivo.extensao hostname
(utilize hostname. Não cheguei a testar com IP. Mas deve dar certo também)



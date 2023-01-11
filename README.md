# Merlin (Prototipo WIP) 

Merlin es un videojuego educativo con reconocimiento de voz desarrollado en [Unity 3D](https://unity3d.com/), dirigido para dispositivos mobiles (actualmente solo Android)

El objetivo es crear un videojuego con instancias educativas en la cual niños y niñas de aproximadamente 7 años puedan practicar y reforzar la lectoescritura. Utiliza un sistema de reconocimiento de voz entrenado por Inteligencia Artificial, puntualmente Speech-To-Text, para evaluar si la frase leída por el usuario fue correcta o incorrecta.

## Sinopsis
Es un juego mobile de lectura y combate en 3D con un estilo low poly estilizado. Lee correctamente nuevos hechizos para aprenderlos y utilizarlos en combate contra tus enemigos.

## Speech-To-Text
Inicialmente desarrollado con el servicio de [Azure](https://azure.microsoft.com/),  [Microsoft Cognitive Services Speech SDK](https://github.com/Azure-Samples/cognitive-services-speech-sdk).

Actualmente utiliza la API de [Vosk](https://github.com/alphacep/vosk-api) Speech To Text, en pos de utilizar un servicio gratuito y offline.

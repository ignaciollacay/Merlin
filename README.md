# WIP Prototipo Merlin con reconocimiento de voz en C# & Unity

Basado en la sample de Microsoft Cognitive Service Speech SDK in [Unity](https://unity3d.com/):
* Utiliza 'Speech recognition with live hypotheses (i.e. interim results)'
* Requiere acceso al micrófono, en Editor y en Builds.
> Note:
> The Speech SDK for Unity supports Windows Desktop (x86 and x64) or Universal Windows Platform (x86, x64, ARM/ARM64), Android (x86, ARM32/64), iOS (x64 simulator, ARM64), Mac (x64) and Linux (x64).
> This sample is targeted for Unity 2020.3 or later.

## Escenas

Apuntamos a dos escenarios básicos de Gameplay:
* Craft: Escena donde se crean nuevos ingredientes y se descubren nuevos hechizos
* Fight: Escena donde pelea contra enemigos utilizando los hechizos.

## Art

Carpeta para trabajar y actualizar el arte. Agregar y organizar libremente.
Ubicación en el proyecto:
> [Project Window] Assets > Art

## Items

Se utilizan Scriptable Objects (SO) para contener la información de cada item.
Ubicación en el proyecto:
> [Project Window] Assets > ScriptableObjects > ItemSO

Project
* Para crear nuevos items: [Project Window] Create > SO > Item
* Escriba el nombre del item SO
> Nota:
> El nombre es de referencia (distinto al nombre en el inspector)
> Camel Case, sin tildes ni espacios.
> Agrupar en la carpeta adecuada

Inspector
En el inspector con el SO seleccionado aparece la información que contiene:
* **Name: Es el nombre del item que aparece en pantalla y es leído por el usuario.**
> **Nota:**
> **Cuidado con la ortografía!**
* Prefab: El prefab que contiene el modelo 3D (FBX)
* Type: El tipo de item. Define si es crafteable o no.
> (Read Only): Se selecciona automáticamente según contenga o no Craftables
> Basic: No pueden ser crafteados.
> Crafted: Compuestos por otros items, producidos con crafteo.
* Craftables: Define 2 items que al ser combinados generan el item actual.
> Nota: Siempre agregar 2 objetos
* Comentarios: Caja de texto para realizar algún comentario (sobre el item o desarrollo)

## Download the Speech SDK for Unity

* **By downloading the Speech SDK for Unity you acknowledge its license, see [Speech SDK license agreement](https://aka.ms/csspeech/license201809).**
* The Speech SDK for Unity is packaged as a Unity asset package (.unitypackage).
  Download it from [here](https://aka.ms/csspeech/unitypackage).

## References

* [Quickstart article on the SDK documentation site](https://docs.microsoft.com/azure/cognitive-services/speech-service/quickstart-csharp-unity)
* [Speech SDK API reference for C#](https://aka.ms/csspeech/csharpref)
* [LUIS documentation](https://docs.microsoft.com/azure/cognitive-services/luis/)
* [LUIS portal](https://luis.ai)

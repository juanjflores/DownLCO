![DownLCO](https://virtualcfdi.com/img/logo404.png)

# DownLCO
Es una pequeña herramienta para descargar la Lista de Contribuyentes Obligados del SAT (LCO), donde se encuentran todos los certificados de los Contribuyentes en México ante el SAT y su validez de obligaciones ante el Servicio de Administración Tributaria, la lista se encuentra en distintos archivos XML cifrados con PKCS7.

## Caracteristicas

- Configuración del "PATH" de descarga del FTP del SAT.
- Configuración del número de archivos que componen la LCO.
- Manejo de excepción de la conexión, si esta se pierde.
- Descompresión de los archivos en formato .tar a .xml
- Verificación de la autenticidad de los archivos XML firmados en PKCS7 mediante OPENSSL.
- Conversión de todos los archivos XML "limpios" de la firma, a un unico archivo de texto plano.
- Configuración de ruta donde se descargan los archivos.
- Personalización de nombre del archivo de texto.

## Dependencias
- Es necesario tener instalado OpenSSL para Windows, ya que es necesario para la verificación de la firma y la exportación del XML limpio.
- Framework 3.5 de .NET
- Desarrollado en Visual Studio 2012

## Instrucciones de Uso
Es necesario crear una carpeta donde se realizara la descarga de los archivos, una vez que de definio una carpeta es necesario actualizar el archivo App.config incluido para modificar la ruta, o dejar la ruta por default "C:\DownLCO\".

## Un ejemplo del archivo App.Config

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <!--RUTA FTP SAT-->
    <add key="RUTAFTPSAT" value="ftp://ftp2.sat.gob.mx/agti_servicio_ftp/cfds_ftp/LCO_"/>
    <!--DIRECTORIO A GUARDAR ARCHIVOS-->
    <add key="DIRECTORIO" value="C:\DownLCO\"/>
    <!--NUM ARCHIVOS A DESCARGAR-->
    <add key="NUMARCHIVOS" value="4"/>
    <!-- NOMBRE ARCHIVO TXTLCO-->
    <add key="NAMETXTLCO" value="C:\\DownLCO\\LCO.txt"/>
  </appSettings>
</configuration>
```

## Licencia
The MIT License (MIT)
Copyright (c) 2014 Infraestructura Multimedia Especializada S.A. de C.V.
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

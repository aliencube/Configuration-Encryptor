# Configuration Encryptor #

**Configuration Encryptor** provides functionality to encrypt and decrypt certain sections on App.config or Web.config.


# Getting Started #

## Installation ##

For the console application, there is no need to install. Instead, [download the zip file](http://github.aliencube.org/Configuration-Encryptor/downloads/ConfigurationEncryptor-1.0.0.0.zip) and unzip to your preferred location.


## Execution ##

**Configuration Encryptor** console application requires the following parameters:

* `/e`|`/d`: Indicates whether to encrypt or decrypt.
* `/c:xxxx`: Specifies configuration filename. Double quote might be necessary for the file path..
* `connectionStrings`: When specified, `connecgtionStrings` section is encrypted/decrypted.
* `appSettings`: When specified, `appSettings` section is encrypted/decrypted.

Here is a sample command:

```
Aliencube.ConfiguratioinEncryptor.ConsoleApp.exe /e /c:"Aliencube.ConfiguratioinEncryptor.ConsoleApp.exe" connectionStrings appSettings
```


# Contribution #

Your contribution is always welcome! All your work should be done in the`dev` branch. Once you finish your work, please send us a pull request on `dev` for review.


# License #

**Configuration Encryptor** is released under [MIT License](http://opensource.org/licenses/MIT).

> The MIT License (MIT)
> 
> Copyright (c) 2014 [aliencube.org](http://aliencube.org)
> 
> Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
> furnished to do so, subject to the following conditions:
> 
> The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
> 
> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

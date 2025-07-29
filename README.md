
# QR Code Generator

  

This is a simple .NET console application that generates a QR code from a URL specified in the configuration and saves it as a PNG image.

  

## How It Works

  

The application reads configuration settings from the `appsettings.json` file to determine the target URL for the QR code and where to save the generated image file. It uses the `QRCoder` library to create the QR code and saves it to the specified path.

  

## Configuration (`appsettings.json`)

  

The `appsettings.json` file contains the following settings:

  

```json
{
	"QRCodeSettings": {
		"TargetUrl": "https://servicebooking.reliancehomecomfort.com",
		"OutputPath": "output/",
		"OutputFileName": "reliance_qr.png"
	}
}
```

  

-  **`TargetUrl`**: The URL that the QR code will point to when scanned.

-  **`OutputPath`**: The directory where the generated QR code image will be saved. The application will create this directory if it does not exist.

-  **`OutputFileName`**: The name of the output PNG image file.

  

## How to Use

  

1.  **Configure the application**: Modify the `appsettings.json` file to set your desired `TargetUrl`, `OutputPath`, and `OutputFileName`.

2.  **Run the application**: Open a terminal in the project's root directory and run the following command:

  

```sh
cd QRCodeGeneratorApp
dotnet run

```

  

3.  **Find the output**: The application will generate the QR code and save it to the location specified in the configuration (e.g., `output/reliance_qr.png`). You will see a confirmation message in the console indicating where the file has been saved.

## Running the Executable Directly

After building the project, you can run the generated executable directly without using the `dotnet run` command. This is useful for running the application in environments where the .NET SDK is not installed (though the .NET Runtime is still required).

1.  **Build the Project**

    First, build the project in either `Debug` or `Release` configuration. A `Release` build is optimized for performance.
    ```sh
    dotnet build --configuration Release
    ```

2.  **Locate the Executable**

    The executable file (`QRCodeGeneratorApp.exe`) will be located in the output directory. For a release build, this will be:
    `bin\Release\net9.0`

3.  **Run the Application**

    Navigate to the output directory and run the executable.
    ```sh
    cd bin\Release\net9.0
    QRCodeGeneratorApp.exe
    ```
    The `appsettings.json` file is configured to be copied to the output directory, so the executable will be able to read it from the same folder.
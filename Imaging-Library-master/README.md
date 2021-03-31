# Imaging Library
PCL Imaging Library especially designed for document scanning like Office Lens and Camscanner. It contains some filters to achieve this. (Canny edge detection, Blob counter, quadrilateral transformation, etc..) 

# Supporting Platforms
Compatible .net platforms: net45 + dnxcore50 + win8 + Xamarin.Android + Xamarin.iOS + Xamarin.Mac

# Nuget Package
Install-Package Imaging.Library
https://www.nuget.org/packages/Imaging.Library

# Code Sample
```csharp
    var imaging = new ImagingManager(source);       //source is PixelMap, you can find samples how to convert image to PixelMap
    imaging.AddFilter(new BicubicFilter(scale));    //Downscaling
    imaging.AddFilter(new CannyEdgeDetector());     //This filter contains Grayscale and Gaussian filter in it
    imaging.Render();                               //Renders the image to use it further use

    var blobCounter = new BlobCounter()
    {
        ObjectsOrder = ObjectsOrder.Size
    };
    imaging.AddFilter(blobCounter);

    imaging.Render();

    //Following code finds largest quadratical blob
    List<Imaging.Library.Entities.Point> corners = null;
    var blobs = blobCounter.GetObjectsInformation();
    foreach (var blob in blobs)
    {
        var points = blobCounter.GetBlobsEdgePoints(blob);

        var shapeChecker = new SimpleShapeChecker();

        if (shapeChecker.IsQuadrilateral(points, out corners))
            break;
    }
    
    imaging.UndoAll();                              //Undo every filters applied
    
    var edgePoints = new EdgePoints();
    edgePoints.SetPoints(corners.ToArray());
    edgePoints = edgePoints.ZoomIn(scale);          //Corrects points that found on downscaled image to original
    imaging.AddFilter(new QuadrilateralTransformation(edgePoints, true));

    imaging.Render();
    
    //imaging.Output gives that extracted rectangle shape from photo. Check out WPF sample how to save it.
```
# Sample Scanning

|Original|Output|
|-----------|-----------|
|![alt source](https://github.com/enginkirmaci/Imaging-Library/blob/master/images/1.jpg)|![alt output](https://github.com/enginkirmaci/Imaging-Library/blob/master/images/2.jpg)|

# Sample Projects
* WPF
    * EdgeDetection
* Xamarin
    * EdgeDetection.Android
    * EdgeDetection.Forms
        * EdgeDetection.Forms (Portable)
        * EdgeDetection.Forms.Android
        * EdgeDetection.Forms.iOS

Note: You can find **AndroidHelper.cs** and **iOSHelper.cs** in **EdgeDetection.Forms (Portable)** which are convert platform specific **bitmap** from/to **PixelMap**.

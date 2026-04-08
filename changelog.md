# Changelog
|Version|Commit|Notes|
|-|-|-|
|0.1.0|`7bcf62712e3b170e13ec9882c59641e7adfe55e8`|Original release|
|0.2.0|`badabe96959031db01a8a01c902012cfa82a6f64`|Added support for Entra ID, keyless authentication|
|0.3.0|`11ff66ed819481321375d7320d6d3fbc41593802`|Re-configured declaration of foundry resource with `FoundryResource` class, morphed from `Deployment`|
|0.4.0|`8f5fd6dbff4052b985ad6f3f2b05ee12ecfb6e57`|Added support for built-in **web search** tool and **code interpreter tool**|
|0.4.1|`fd0008e617379b551d945ef1debaadd8cb56afe7`|`Expires` property of `TokenCredential` is now a `DateTimeOffset`, not just a `DateTime`|
|0.4.2|`9593852bbd8424d130f9c45bf8ca3d6876cfb570`|`HttpCompletionOption.ResponseHeadersRead` added to calls to Foundry (supports long-living requests)|
|0.5.0|`c55f7478c1555299b6939d2e1ed5b4cd5083097c`|Added ability to use `background` parameter of responses and later retrieve them (run asynchronously)|
|0.6.0|`27a8deaddcd5f57b30b837f5357afc5841731394`|Added `incomplete` status to `ResponseStatus` and added *blocked* properties to Response|
|0.6.1|`991485e54d0eae02899ca615fa4b8141987fbf5c`|Fixed JArray conversion in last release that checked for blocked prompts/completions|
|0.7.0|`f34f595c5a3b4ef6dc8c0cfc2569e9d86d03784f`|Fixed block detection logic and consolidated to only one "blocked" property on the Response|
|0.8.0|`0f775d21d77c64973da7392a2b177a7b82904d6d`|Added `CreatedAt` to Response|
|0.9.0|`e7b285ee08752732369708910eb7fdcb2dad7f83`|Added ability to include images via `InputImage` and recorded `model` property in response|
|0.10.0|`f585b61d85506573fc88a90a96ff81f93d854760`|Added `InputImageDetail` property to `InputImage`|
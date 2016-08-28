# AssetsResizeMagick
svgから複数サイズのpngを生成します。(実際はsvg以外でも大丈夫)  

## 使い方
### 基本的な使い方
実行ファイルに変換したいファイルをドロップします。  

### 設定xml
settings.xmlは実行ファイルと同じディレクトリに置いてください。  

```xml
<Types>
  <Type name="UWP Square44x44Logo" folderName="Assets">
    <Image x="44" y="44">Square44x44Logo.scale-100.png</Image>
    <Image x="55" y="55">Square44x44Logo.scale-125.png</Image>
    <Image x="66" y="66">Square44x44Logo.scale-150.png</Image>
    <Image x="88" y="88">Square44x44Logo.scale-200.png</Image>
    <Image x="176" y="176">Square44x44Logo.scale-400.png</Image>
  </Type>
</Types>
```
+ Type要素  
name ... コンソールに表示する名前(必須)  
folderName ... 出力するフォルダ名  

+ Image要素  
x ... 変換後の横のサイズ(必須)  
y ... 変換後の縦のサイズ(必須)  
folderName ... 出力するフォルダ名  
要素の内容 ... 変換後のファイル名、無ければ元のファイル名と同じものになる  

## LICENSE
[Magick.NET](http://magick.codeplex.com/license "Magick.NET")  

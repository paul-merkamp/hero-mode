<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.10" tiledversion="1.10.2" name="FG" tilewidth="30" tileheight="50" tilecount="11" columns="0">
 <grid orientation="orthogonal" width="1" height="1"/>
 <tile id="3">
  <image width="10" height="10" source="../Tiles/Wall_Stone.png"/>
  <objectgroup draworder="index" id="6">
   <object id="7" x="0" y="0" width="10" height="10"/>
  </objectgroup>
 </tile>
 <tile id="4">
  <image width="10" height="10" source="../Tiles/Wall_Stone_Top.png"/>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="10" height="10"/>
  </objectgroup>
 </tile>
 <tile id="5">
  <image width="10" height="10" source="../Tiles/Hole.png"/>
 </tile>
 <tile id="6">
  <image width="10" height="16" source="../Tiles/Oven.png"/>
  <objectgroup draworder="index" id="4">
   <object id="5" x="0" y="0" width="10" height="16"/>
   <object id="7" x="0" y="0" width="10" height="16"/>
  </objectgroup>
 </tile>
 <tile id="7">
  <image width="10" height="50" source="../Tiles/Table.png"/>
  <objectgroup draworder="index" id="4">
   <object id="3" x="0" y="0" width="10" height="49.9124"/>
   <object id="6" x="0" y="0" width="10" height="50"/>
   <object id="8" x="0" y="0" width="10" height="50"/>
  </objectgroup>
 </tile>
 <tile id="8">
  <image width="10" height="50" source="../Tiles/Table2.png"/>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="10" height="50"/>
  </objectgroup>
 </tile>
 <tile id="9">
  <image width="30" height="30" source="../Tiles/Lava_full.gif"/>
  <objectgroup draworder="index" id="2">
   <object id="8" x="0" y="0" width="30" height="30">
    <polygon points="0,0 30,0 30,30 0,30"/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="9" duration="100"/>
   <frame tileid="9" duration="100"/>
  </animation>
 </tile>
 <tile id="10">
  <image width="10" height="10" source="../Tiles/Bar.png"/>
  <objectgroup draworder="index" id="3">
   <object id="2" x="0" y="0" width="10" height="10">
    <polygon points="0,0 10,0 10,10 0,10"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="14">
  <image width="30" height="30" source="../Tiles/Shining_bar_bg_empty.png"/>
 </tile>
 <tile id="15">
  <image width="30" height="30" source="../Tiles/Shining_bar_bg.png"/>
 </tile>
 <tile id="16">
  <image width="10" height="10" source="../Tiles/Stone.png"/>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="10" height="10">
    <polygon points="0,0 10,0 10,10 0,10"/>
   </object>
  </objectgroup>
 </tile>
</tileset>

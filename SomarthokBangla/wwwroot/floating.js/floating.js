!function(e,t){"object"==typeof exports&&"undefined"!=typeof module?module.exports=t():"function"==typeof define&&define.amd?define(t):e.floating=t()}(this,function(){"use strict";function e(){var e=arguments.length>0&&void 0!==arguments[0]?arguments[0]:{},t=e.content,n=void 0===t?"👌":t,o=e.number,i=void 0===o?1:o,a=e.duration,d=void 0===a?10:a,r=e.repeat,l=void 0===r?"infinite":r,f=e.direction,m=void 0===f?"normal":f,s=e.size,c=void 0===s?2:s,v=document.createElement("style");v.id="floating-style",document.getElementById("floating-style")||document.head.appendChild(v);for(var h="@keyframes float{",u=200,p=0;p<=u;p++)h+=100*p/u+"%{transform:translate("+10*Math.sin(p/10)+"vw,"+(p*(-120/u)+110)+"vh)}";h+="}",document.getElementById("floating-style").innerHTML=".float-container{width:100vw;height:100vh;overflow:hidden;position:absolute;top:0;left:0;pointer-events:none}.float-container div *{width:1em;height:1em}"+h;var y=document.createElement("div");y.className="float-container";for(var g=function(e){var t=document.createElement("div");t.innerHTML=n;var o=c;c instanceof Array&&(o=Math.floor(Math.random()*(c[1]-c[0]+1))+c[0]),t.style.cssText="position:absolute;left:0;font-size:"+o+"em;transform:translateY(110vh);animation:float "+d+"s linear "+e*Math.random()+"s "+l+" "+m+";margin-left:"+100*Math.random()+"vw;",t.addEventListener("animationend",function(e){"float"===e.animationName&&y.removeChild(t)}),y.appendChild(t)},M=0;M<i;M++)g(M);document.body.appendChild(y)}return e});
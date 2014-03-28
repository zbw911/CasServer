Dev.All / CASServer 
==================================================================================================

本项目实现了部分 CAS 协议.

<br/>协议部分借鉴了 http://jasigemu.codeplex.com/ 项目中大部分模拟协议代码.



地区数据 JS 引用 
http://xiaocai.name/index.php?uri=/blog/info/101




1,其中  UserExtend 包含 decimal(11,0) 是为了兼容过去的 Discuz 中的 uid 字段而做出的妥协,实际上,这对于其它项目来讲是没有意义的,这也仅仅是满足小部分人的需求,在后面的版本中将可能去除.
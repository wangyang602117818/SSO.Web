# sso实现原理
## 基于Json Web Token(JWT)的token认证
```
1: JWT Token是目前最流行的跨域身份验证解决方案, Token分成3个部分:Header(包含算法),Payload(包含数据),Signature(包含签名,防止数据篡改)  
2: 整个系统中Token会保存: 用户编号,用户名称,用户语言,token的来源,用户ip  
3: 安全,Token中保存了用户ip,当有用户拷贝别人的token使用,会导致ip验证失败; token中使用了签名算法,当有用户修改了token会导致签名验证失败.  
```
## SSO认证过程
```
1. 当用户第一次访问 A站点,因为没有token,系统会重定向到 SSO站点,在 SSO站点认证成功之后随即在SSO站点设置cookie并跳回A站点,并且在url上带上ticket(这个ticket过期时间15秒,可以设置),A站点拿到ticket,解析出里面的用户id,然后用该id去SSO站点获取JWTToken 然后把该token设置成A站点的cookie,之后A站点的每次请求都在header中带上该token(Authorization:token),或者在请求中带上cookie都能通过验证,至此A站点完成登录
2. 当用户第一次访问 B站点,因为没有token,系统会重定向到 SSO站点,因为之前在 SSO站点设置了cookie,这次重定向到SSO站点带上了SSO站点的cookie,SSO站点判定用户登录过,不需要用户再次认证,直接跳到B站点并且在url上带上ticket,B站点拿到ticket,解析出里面的用户id,然后用该id去SSO站点获取JWTToken ,然后把该token设置成B站点的cookie,之后B站点的每次请求都在header中带上该token(Authorization:token),或者在请求中带上cookie都能通过验证,至此B站点完成登录
3. SSO退出,用户每次登录成功之后,都会在SSO站点留下登录成功的站点地址(A站点,B站点),当用户退出的时候,先重定向到SSO站点,SSO站点先清除自身的cookie,重定向到A站点,A站点收到退出请求,清除自身的cookie,A站点发出重新到B站点的请求,B站点收到退出请求,清除自身的cookie; 全部清除完毕,跳到sso站点的登录界面,完成SSO退出
4. 整个SSO认证步骤都封装在Nuget的SSO.Util.Client.dll和npm的sso-util中
```

import { useEffect, useState, ComponentType } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { CookiesKeysCollection, getCookies, setCookies } from '../configuration';
import AccountService from '@/api/instance/account';
import { setUserInfo } from '@/store/redux/account';
import { UrlRouteCollection } from '@/common/url-route-collection';

export const routeGuard = (Component: ComponentType<any>) =>
  function Comp(props: any) {
    const location = useLocation();
    const navigate = useNavigate();

    const [isLogin, setIsLogin] = useState(false);

    const dispatch = useDispatch();
    const objectToQueryString = (obj: any) => {
      return Object.keys(obj).map(key => `${encodeURIComponent(key)}=${encodeURIComponent(obj[key])}`).join('&');
    };    

    const checkAuthentication = async () => {
      try {
        const token = getCookies(CookiesKeysCollection.TOKEN_KEY);
        if ( !token ) {
            throw new Error("No token found");
        }

        const res = await AccountService.GetMyInfo();
        dispatch(setUserInfo(res));
        setIsLogin(true);
      } catch {
        dispatch(setUserInfo(null));
        setCookies(CookiesKeysCollection.RETURN_URL, `${location.pathname}?${objectToQueryString(location.search)}`);
        navigate(UrlRouteCollection.Login);
      }
    };

    useEffect(() => {
        checkAuthentication();
    })

    return isLogin && <Component {...props} />;
  };

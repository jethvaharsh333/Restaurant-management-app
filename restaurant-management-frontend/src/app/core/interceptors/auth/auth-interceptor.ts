import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  req = req.clone({
    withCredentials: true
  });

  console.log("API REQUEST: ");
  console.log(req);

  return next(req);
};

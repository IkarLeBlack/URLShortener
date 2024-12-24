import {UserModel} from './user.interface';

export interface UrlModel {
  id: number;
  originalUrl: string;
  shortUrl: string;
  createdBy: string;
  createdDate: Date;
  userId?: number;
  user?: UserModel;
}

import {UrlModel} from './url.interface';
import {RoleModel} from './role.interface';

export interface UserModel {
  id: number;
  username: string;
  passwordHash: string;
  roleId: number;
  role?: RoleModel;
  urls?: UrlModel[];
}

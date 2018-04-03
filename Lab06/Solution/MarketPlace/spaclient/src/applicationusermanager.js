import { UserManager } from 'oidc-client'

class ApplicationUserManager extends UserManager {
  constructor () {
    super({
      authority: 'http://localhost:5002',
      client_id: 'marketplacejs',
      redirect_uri: 'http://localhost:5001/#/callback/#',
      response_type: 'id_token token',
      scope: 'openid profile marketplaceapi',
      post_logout_redirect_uri: 'http://localhost:5001/index.html'
    })
  }

  async login () {
    await this.signinRedirect()
    return this.getUser()
  }

  async logout () {
    return this.signoutRedirect()
  }
}

const applicationUserManager = new ApplicationUserManager()
export { applicationUserManager as default }

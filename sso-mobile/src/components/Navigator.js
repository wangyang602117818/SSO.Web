import React from 'react';
import { Grid } from 'antd-mobile';
import { axios, urls } from '../config/http';
import '../css/navigator.css';
class Navigator extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            datas: []
        };
    }
    componentDidMount() {
        axios.get(urls.navigation.geturlmeta).then(response => {
            if (response.code == 0) {
                this.setState({ datas: response.result });
            }
        });
    }
    itemClick(item,index){
       window.open(item.BaseUrl);
    }
    render() {
        return (
            <div className="navigator">
                <div className="nav_top">
                    <input type="text" placeholder="search" />
                </div>
                <div className="nav_content">
                    <div className="sub_title">导航列表 </div>
                    <Grid data={this.state.datas}
                        columnNum={3}
                        onClick={this.itemClick}
                        renderItem={dataItem => (
                            <div>
                                <img className="nav_logo" src={dataItem.LogoUrl} alt={dataItem.BaseUrl} />
                                <div className="nav_title">
                                    <span>{dataItem.Title}</span>
                                </div>
                            </div>
                        )}
                    />
                </div>
            </div>
        )
    }
}
export default Navigator;

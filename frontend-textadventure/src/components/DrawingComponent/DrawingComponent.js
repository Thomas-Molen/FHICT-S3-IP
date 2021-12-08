import { React, useEffect } from 'react';
import CanvasDraw from "react-canvas-draw";
import useState from 'react-usestateref';
import { UseFetchWrapper } from '../../helpers'
import { compressToBase64, decompressFromBase64 } from 'lz-string'
import './DrawingComponent.css';
import { Button, Row, Col, Container } from 'react-bootstrap'
import { Icon } from '@iconify/react';
import ReactTooltip from 'react-tooltip';

export function DrawingComponent({ adventurerId }) {
    const fetchWrapper = UseFetchWrapper();
    const [keyValue, setKeyValue] = useState(1);
    //drawingCanvas
    const [drawingCanvas, setDrawingCanvas] = useState();
    const [drawing, setDrawing] = useState("");


    useEffect(() => {
        fetchWrapper.get(`Adventurer/get-drawing/${adventurerId}`)
            .then((response) => {
                setDrawing(decompressFromBase64(response.drawing));
            })
    }, [])

    return (
        <>
            <div className="DrawingCanvas mb-5">
                <div className="row-fluid m-2">
                    <Icon icon="feather:save" color="#585858" width="30" className="pointer drawingOption me-2"
                        onClick={() => {
                            var drawing = drawingCanvas.getSaveData();
                            setDrawing(drawing);
                            fetchWrapper.post(`Adventurer/save-drawing/${adventurerId}`, { drawing: compressToBase64(drawing) })
                                .then(() => {
                                    setDrawing(drawing);
                                })
                            console.log(drawing)
                        }} />
                    <Icon icon="ci:trash-empty" color="#585858" width="30" className="pointer drawingOption me-2"
                        onClick={() => {
                            drawingCanvas.eraseAll();
                        }} />
                    <Icon icon="ic:baseline-undo" color="#585858" width="30" className="pointer drawingOption me-2"
                        onClick={() => {
                            drawingCanvas.undo();
                        }} />
                    <Icon icon="mdi:reload-alert" color="#585858" width="30" className="pointer drawingOption"
                        onClick={() => {
                            fetchWrapper.post(`Adventurer/save-drawing/${adventurerId}`, { drawing: compressToBase64(drawingCanvas.getSaveData()) })
                                .then(() => {
                                    setDrawing(drawingCanvas.getSaveData);
                                    setKeyValue(keyValue + 1);
                                })
                        }} />
                </div>
                <hr className="m-0" />
                <div className="d-flex">
                    <CanvasDraw
                        className="NotePadCanvas"
                        key={keyValue}
                        ref={canvasDraw => (setDrawingCanvas(canvasDraw))}
                        brushColor={"#585858"}
                        backgroundColor={"#252525"}
                        catenaryColor={"ffffff"}
                        hideGrid
                        enablePanAndZoom
                        mouseZoomFactor={0.001}
                        lazyRadius={0}
                        brushRadius={2}
                        canvasWidth={1000}
                        canvasHeight={360}
                        saveData={drawing}
                        immediateLoading={true} />
                </div>
            </div>



            {/* <Container fluid>
                <div className="NotePad d-flex characterStats">
                    <Col>
                        <div className="d-flex">
                            <Icon icon="feather:save" color="white" width="30" className="pointer"
                                onClick={() => {
                                    var drawing = drawingCanvas.getSaveData();
                                    setDrawing(drawing);
                                    fetchWrapper.post(`Adventurer/save-drawing/${adventurerId}`, { drawing: compressToBase64(drawing) })
                                        .then(() => {
                                            setDrawing(drawing);
                                        })
                                    console.log(drawing)
                                }} />
                            <Icon icon="ci:trash-empty" color="white" width="30" className="pointer"
                                onClick={() => {
                                    drawingCanvas.eraseAll();
                                }} />
                            <Icon icon="ic:baseline-undo" color="white" width="30" className="pointer"
                                onClick={() => {
                                    drawingCanvas.undo();
                                }} />
                            <Icon icon="mdi:reload-alert" color="white" width="30" className="pointer"
                                onClick={() => {
                                    fetchWrapper.post(`Adventurer/save-drawing/${adventurerId}`, { drawing: compressToBase64(drawingCanvas.getSaveData()) })
                                        .then(() => {
                                            setDrawing(drawingCanvas.getSaveData);
                                            setKeyValue(keyValue + 1);
                                        })
                                }} />
                        </div>
                        <hr className="m-0" />
                        <CanvasDraw
                            className="NotePadCanvas"
                            key={keyValue}
                            ref={canvasDraw => (setDrawingCanvas(canvasDraw))}
                            brushColor={"#585858"}
                            backgroundColor={"#252525"}
                            catenaryColor={"ffffff"}
                            hideGrid
                            enablePanAndZoom
                            mouseZoomFactor={0.001}
                            lazyRadius={0}
                            brushRadius={2}
                            canvasWidth={1000}
                            saveData={drawing}
                            immediateLoading={true} />
                    </Col>
                </div>
            </Container> */}
        </>
    )
}
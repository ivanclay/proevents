import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  evento = {} as Evento;
  estadoSalvar = 'post';
  form!: FormGroup;

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
        isAnimated: true,
        adaptivePosition: true,
        dateInputFormat: 'DD/MM/YYYY hh:mm a',
        containerClass: 'theme-default',
        showWeekNumbers: false
    };
  }


  constructor(private fb: FormBuilder,
              private localeService: BsLocaleService,
              private router: ActivatedRoute,
              private eventoService: EventoService,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService)
  {
    this.localeService.use("pt-br");
  }

  public carregarEvento(): void {
    const eventoIdParam = this.router.snapshot.paramMap.get('id');

    if(eventoIdParam !== null){
      this.spinner.show();
      this.estadoSalvar = 'put';
      this.eventoService.getEventoById(+eventoIdParam).subscribe(
        (evento: Evento) => {
          this.evento = {...evento};
          this.form.patchValue(this.evento);
        },
        (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao tentar carregar evento.','Erro');
          console.error(error);
        },
        () => this.spinner.hide(),
      );
    }
  }

  ngOnInit() {
    this.carregarEvento();
    this.validation();
  }

  public validation():void{
    this.form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imagemURL: ['', [Validators.required]],
      telefone: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      lotes: this.fb.array([])
    });
  }

  adicionarLote(): void {
    this.lotes.push(
      this.criarLote({id: 0} as Lote)
    );
  }

  private criarLote(lote: Lote) {
    return this.fb.group({
      id: [lote.id, Validators.required],
      nome: [lote.nome, Validators.required],
      preco: [lote.preco, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim]
    });
  }

  public resetForm(): void{
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl) : any{
    return {'is-invalid': campoForm?.errors && campoForm?.touched }
  }

  public salvarAlteracao(): void{
    this.spinner.show();
    if(this.form.valid){

      //Aqui foi refatorado -> Não funcionou pra mim.
      //rever no final.
      if(this.estadoSalvar === 'post'){
        this.evento = {...this.form.value};
        this.eventoService.post(this.evento).subscribe(
          () => this.toastr.success('Evento cadastrado.','Sucesso'),
          (error: any) => {
            console.error(error);
            this.spinner.hide();
            this.toastr.error('Ocorreu um erro.','Erro');
          },
          () => this.spinner.hide(),
        );
      }else{
        this.evento = {id: this.evento.id, ...this.form.value};
        this.eventoService.put(this.evento).subscribe(
          () => this.toastr.success('Evento cadastrado.','Sucesso'),
          (error: any) => {
            console.error(error);
            this.spinner.hide();
            this.toastr.error('Ocorreu um erro.','Erro');
          },
          () => this.spinner.hide(),
        );
      }
    }
  }

}
